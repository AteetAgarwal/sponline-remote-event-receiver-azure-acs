using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePointAddInOnlineWeb.Services
{
    public class RemoteEventReceiver : IRemoteEventService
    {
        /// <summary>
        /// Handles events that occur before an action occurs, such as when a user adds or deletes a list item.
        /// </summary>
        /// <param name="properties">Holds information about the remote event.</param>
        /// <returns>Holds information returned from the remote event.</returns>
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
            {
                if (clientContext != null)
                {
                    clientContext.Load(clientContext.Web);
                    clientContext.ExecuteQuery();
                }
            }

            return result;
        }

        /// <summary>
        /// Handles events that occur after an action occurs, such as after a user adds an item to a list or deletes an item from a list.
        /// </summary>
        /// <param name="properties">Holds information about the remote event.</param>
        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            if (properties.EventType == SPRemoteEventType.ItemAdded)
            {
                using (ClientContext clientContext = TokenHelper.CreateRemoteEventReceiverClientContext(properties))
                {
                    if (clientContext != null)
                    {
                        try
                        {
                            List lstDemoeventReceiver = clientContext.Web.Lists.GetByTitle(properties.ItemEventProperties.ListTitle);
                            ListItem itemDemoventReceiver = lstDemoeventReceiver.GetItemById(properties.ItemEventProperties.ListItemId);
                            itemDemoventReceiver["Title"] = " updated by remote event reciever =>; " + DateTime.Now.ToString();
                            itemDemoventReceiver.Update();
                            clientContext.ExecuteQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.ToString());
                        }
                    }
                }
            }
        }
    }
}
