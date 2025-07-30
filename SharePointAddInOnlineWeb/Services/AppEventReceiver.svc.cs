using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharePointAddInOnlineWeb.Services
{
    public class AppEventReceiver : IRemoteEventService
    {
        /// <summary>
        /// Handles app events that occur after the app is installed or upgraded, or when app is being uninstalled.
        /// </summary>
        /// <param name="properties">Holds information about the app event.</param>
        /// <returns>Holds information returned from the app event.</returns>
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            if (properties.EventType == SPRemoteEventType.AppInstalled)
            {
                using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, useAppWeb: false))
                {
                    if (clientContext != null)
                    {
                        List list = clientContext.Web.Lists.GetByTitle("RER");

                        var rer = new EventReceiverDefinitionCreationInformation
                        {
                            EventType = EventReceiverType.ItemAdded,
                            ReceiverName = "RemoteEventReceiverItemAdded",
                            ReceiverUrl = "https://sharepointaddinonlineweb20250729233721-hcgcg5edc0hkbnbf.canadacentral-01.azurewebsites.net/Services/RemoteEventReceiver.svc",
                            SequenceNumber = 1000
                        };

                        list.EventReceivers.Add(rer);
                        clientContext.ExecuteQuery();
                    }
                }
            }

            return result;
        }

        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            // Not used in AppInstalled
        }

    }
}
