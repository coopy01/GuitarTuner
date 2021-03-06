namespace LuisBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using System.Text;

    using System.Net.Http;
    using System.Reflection;

    [Serializable]
    
    [LuisModel("[TODO: input Id of LUIS App]", "[TODO: input Password of LUIS App]")]
    public class RootLuisDialog : LuisDialog<object>
    {
       
        /// <summary>
        /// Need to override the LuisDialog.MessageReceived method so that we can detect when the user invokes the skill without
        /// specifying a phrase, for example: "Open <invocation>", or "Ask <invocation>". In these cases, the message received will be an empty string.
        /// I would much prefer that the inltentless launch be handled explicitly rather than implicitly through the message 
        /// equals emtpy string method. Currently unable to implement this to my satisfaction so leaving this kludge in place
        /// </summary>
        /// <param name="context"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            if (message.Text == null)
           {
                //Because there is no message we know/assume it is a launch
               await Launch(context, null);
            }
            else
            {
                await base.MessageReceived(context, item);
            }
        }

        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var response = context.MakeMessage();
            response.Text = $"I am the none intent.";
            response.Speak = $"Sorry, I don't know your intentions. Say 'help' if you need assistance.";
            response.InputHint = InputHints.AcceptingInput;

            await context.PostAsync(response);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("play tone")]
        public async Task Disambiguate(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            EntityRecommendation entityRec = null;
            if (result != null) {
                IList<EntityRecommendation> entityRecList = result.Entities;
                if (entityRecList != null)
                {
                    if (entityRecList.Count > 0)
                    {
                        entityRec = entityRecList[0];
                    }
                }
            }

            var message = context.MakeMessage() as IMessageActivity;

            if (entityRec == null)
            {
                CardLowE(message);
            } else {

                switch (entityRec.Entity.ToLower())
                {
                    case "low e":
                        CardLowE(message);
                        break;
                    case "a":
                        CardA(message);
                        break;
                    case "d":
                        CardD(message);
                        break;
                    case "g":
                        CardG(message);
                        break;
                    case "b":
                        CardB(message);
                        break;
                    case "high e":
                        CardHighE(message);
                        break;
                    default:
                        CardLowE(message);
                        break;
                }
            }
            await context.PostAsync(message);
        }
        [LuisIntent("YesIntent")]
        public async Task Confirm(IDialogContext context, LuisResult result)
        {
            var response = context.MakeMessage();
            response.Text = $"I am the confirm intent.";
            response.Speak = $"I confirm your confirmation. Say 'help' if you need assistance.";
            response.InputHint = InputHints.AcceptingInput;

            await context.PostAsync(response);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            var response = context.MakeMessage();
            response.Text = $"I am no intent at all.";
            response.Speak = $"Sorry, you don't have any intentions. Say 'help' if you need assistance.";
            response.InputHint = InputHints.AcceptingInput;

            await context.PostAsync(response);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("FakeLaunchIntent")]
        public async Task FakeLaunch(IDialogContext context, LuisResult result)
        {
            var response = context.MakeMessage();
            response.Text = $"Fake Launch Intent";
            response.Speak = $"Lunch? I thought you said LAUNCH.";
            response.InputHint = InputHints.AcceptingInput;

            await context.PostAsync(response);

            context.Wait(this.MessageReceived);
        }
        [LuisIntent("Microsoft.Launch")]
        public async Task Launch(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage() as IMessageActivity;
            message.Speak = "It's guitar tuning time!";
            message.Summary = "Which note do you want? ";
            message.Text = "6-string standard tuning:";
            message.Attachments = new List<Attachment>(){
                new HeroCard("Which one do you want?", "Make a selection", "low e, a, d, g, b, or high e", new List<CardImage>()
                {
                    new CardImage() { Url = "", Alt = "Hero Card Image Alt" }
                }, new List<CardAction>()
                {
                    new CardAction("openUrl", "Reference Document", null, "https://docs.microsoft.com/en-us/cortana/design-guides/card-design-best-practices")
                }).ToAttachment()
            };
            message.InputHint = InputHints.ExpectingInput;
            await context.PostAsync(message);
        }

        [LuisIntent("ShowCardVersionNumbers")]
        public async Task Version(IDialogContext context, LuisResult result)
        {
            Assembly thisAssem = typeof(RootLuisDialog).Assembly;
            AssemblyName thisAssemName = thisAssem.GetName();
            var message = context.MakeMessage() as IMessageActivity;
            message.Speak = "This is version " + thisAssemName.Version + " of " + thisAssemName.Name;
            message.Summary = "Card Demo Version Check:";
            message.Text = message.Speak;
            message.InputHint = InputHints.AcceptingInput;
            await context.PostAsync(message);
        }

        void CardLowE(IMessageActivity message)
        {
            message.Speak = "Playing a low E";
            message.Summary = "Playing a low E";
            message.Text = "Low E: 82.41Hz";
            message.InputHint = InputHints.AcceptingInput;
            message.Attachments = new List<Attachment>() {
                new AudioCard("StreamTitle","Subtitle","Text", null, new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "[TODO: Link to public location of your Low E tone. 82.41 Hz]"
                    }
                },null, true, true, true, null).ToAttachment()
            };
        }
        void CardA(IMessageActivity message)
        {
            message.Speak = "Playing an A";
            message.Summary = "Playing an A";
            message.Text = "A: 110.00Hz";
            message.InputHint = InputHints.AcceptingInput;
            message.Attachments = new List<Attachment>() {
                new AudioCard("StreamTitle","Subtitle","Text", null, new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "[TODO: Link to public location of your A tone. 110.00 Hz]"
                    }
                },null, true, true, true, null).ToAttachment()
            };
        }
        void CardD(IMessageActivity message)
        {
            message.Speak = "Playing a D";
            message.Summary = "Playing a D";
            message.Text = "D: 146.83Hz";
            message.InputHint = InputHints.AcceptingInput;
            message.Attachments = new List<Attachment>() {
                new AudioCard("StreamTitle","Subtitle","Text", null, new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "[TODO: Link to public location of your D tone. 146.83 Hz]"
                    }
                },null, true, true, true, null).ToAttachment()
            };
        }
        void CardG(IMessageActivity message)
        {
            message.Speak = "Playing a G";
            message.Summary = "Playing a G";
            message.Text = "G: 196.00Hz";
            message.InputHint = InputHints.AcceptingInput;
            message.Attachments = new List<Attachment>() {
                new AudioCard("StreamTitle","Subtitle","Text", null, new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "[TODO: Link to public location of your G tone. 196.00 Hz]"
                    }
                },null, true, true, true, null).ToAttachment()
            };
        }
        void CardB(IMessageActivity message)
        {
            message.Speak = "Playing a B";
            message.Summary = "Playing a B";
            message.Text = "B: 246.94Hz";
            message.InputHint = InputHints.AcceptingInput;
            message.Attachments = new List<Attachment>() {
                new AudioCard("StreamTitle","Subtitle","Text", null, new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "[TODO: Link to public location of your B tone. 246.94 Hz]"
                    }
                },null, true, true, true, null).ToAttachment()
            };
        }
        void CardHighE(IMessageActivity message)
        {
            message.Speak = "Playing a high E";
            message.Summary = "Playing a high E";
            message.Text = "High E: 329.63Hz";
            message.InputHint = InputHints.AcceptingInput;
            message.Attachments = new List<Attachment>() {
                new AudioCard("StreamTitle","Subtitle","Text", null, new List<MediaUrl>()
                {
                    new MediaUrl()
                    {
                        Url = "[TODO: Link to public location of your E tone. 329.63 Hz]"
                    }
                },null, true, true, true, null).ToAttachment()
            };
        }
        [LuisIntent("Utilities.Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage() as IMessageActivity;
            message.Summary = "Hi! Try asking me things like 'play a low e', 'b', 'give me a g' or 'play a high e'";
            message.Speak = @"<speak version=""1.0"" xml:lang=""en-US"">Hi! Try asking me things like 'play a low e'</speak>";
            message.InputHint = InputHints.ExpectingInput;
            message.Text = "Help";
            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }        
    }
    
    static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            var output = string.Empty;
            if (!string.IsNullOrEmpty(input))
            {
                output = input.Substring(0, 1).ToUpper() + input.Substring(1);
            }
            // Strip out periods 
            output = output.Replace(".", "");

            return output;
        }
    }
    
}

