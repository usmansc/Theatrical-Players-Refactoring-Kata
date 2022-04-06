﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string createTag(string tag, string text)
        {
            return string.Format("<{0}>{1}</{0}>", tag, text);
        }
        public string PrintAsHtml(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = "<html>\n";
            result += createTag("h1", string.Format("Statement for {0}", invoice.Customer));
            CultureInfo cultureInfo = new CultureInfo("en-US");
            result += "\n<table>\n";
            result += "<tr><th>play</th><th>seats</th><th>cost</th></tr>\n";
            for (var index = 0; index < invoice.Performances.Count; index++)
            {
                var perf = invoice.Performances[index];
                var play = plays[perf.PlayID];
                var thisAmount = 0;
                switch (play.Type)
                {
                    case "tragedy":
                        thisAmount = 40000;
                        if (perf.Audience > 30)
                        {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }

                        break;
                    case "comedy":
                        thisAmount = 30000;
                        if (perf.Audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }

                        thisAmount += 300 * perf.Audience;
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }

                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int) Math.Floor((decimal) perf.Audience / 5);

                // print line for this order
                result += string.Format(cultureInfo, "<tr><td>{0}</td><td>{1}</td><td>{2:C}</td></tr>\n",
                    play.Name, perf.Audience, Convert.ToDecimal(thisAmount / 100));

                totalAmount += thisAmount;
            }

            result += "</table>\n";
            result += createTag("p",string.Format(cultureInfo, "Amount owed is <em>{0:C}</em>", Convert.ToDecimal(totalAmount / 100)));
            result += "\n";
            var credits = createTag("em", volumeCredits.ToString());
            result += createTag("p",string.Format("You earned {0} credits", credits));
            result += "\n";
            result += "</html>";
            return result;
        }
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = 0;
                switch (play.Type) 
                {
                    case "tragedy":
                        thisAmount = 40000;
                        if (perf.Audience > 30) {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }
                        break;
                    case "comedy":
                        thisAmount = 30000;
                        if (perf.Audience > 20) {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }
                        thisAmount += 300 * perf.Audience;
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
            result += string.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += string.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }
        
    }
}
