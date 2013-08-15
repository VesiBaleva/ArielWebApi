using ArielWebRecipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ArielWebRecipe.WebApi.Libraries
{
    public class PubnubPublisher
    {
        public static void PubnubThread(List<PreparationStep> steps, string channel, string sessionKey)
        {
            PubnubAPI pubnub = new PubnubAPI(
                    "pub-c-26f81a7d-18b7-4472-976d-f6d6ba477ee0",               // PUBLISH_KEY
                    "sub-c-8dc89202-0580-11e3-8dc9-02ee2ddab7fe",               // SUBSCRIBE_KEY
                    "sec-c-ZWMwYzA1N2MtNTRkYy00ZjhkLTg0NGItNTdmMDJhNDA5MWY3",   // SECRET_KEY
                    true);

            foreach (var step in steps)
            {
                Thread.Sleep(step.PreparationTime * 1000);
                string message = "Step " + step.Order + " Completed";

                pubnub.Publish(channel, message);
            }
        }
    }
}