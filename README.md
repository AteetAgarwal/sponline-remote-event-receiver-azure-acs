# SharePoint Provider-Hosted Add-in: Remote Event Receiver Setup Guide

This guide explains how to set up a Remote Event Receiver (RER) for a SharePoint Provider-Hosted Add-in using a client secret generated via SharePoint's AppRegNew.aspx (Azure ACS), **not** Azure AD.

## Prerequisites
- SharePoint Online tenant admin access
- Visual Studio (or compatible IDE)
- Access to [AppRegNew.aspx](https://<your-sharepoint-site>/_layouts/15/AppRegNew.aspx)
- Access to [AppInv.aspx](https://<your-sharepoint-site>/_layouts/15/AppInv.aspx)

## 1. Register the Add-in (AppRegNew.aspx)
1. Go to `https://<your-sharepoint-site>/_layouts/15/AppRegNew.aspx`.
2. Click **Generate** for Client Id and Client Secret.
3. Fill in:
   - **Title**: Your app name
   - **App Domain**: Your provider-hosted app domain (e.g., `myapp.azurewebsites.net`)
   - **Redirect URI**: The HTTPS URL of your app (e.g., `https://myapp.azurewebsites.net`)
4. Click **Create**. Save the Client Id and Client Secret.

## 2. Grant Permissions (AppInv.aspx)
1. Go to `https://<your-sharepoint-site>/_layouts/15/AppInv.aspx`.
2. Enter the **Client Id** and click **Lookup**.
3. Paste the following XML in the **Permission Request XML** box (modify as needed):
   ```xml
   <AppPermissionRequests>
     <AppPermissionRequest Scope="http://sharepoint/content/sitecollection" Right="FullControl" />
   </AppPermissionRequests>
   ```
4. Click **Create** and then **Trust It**.

## 3. Configure the Provider-Hosted Add-in
- In your Visual Studio project, update `web.config` (or `appsettings.json`) with:
  - `ClientId` (from AppRegNew)
  - `ClientSecret` (from AppRegNew)
  - `Realm` (can be found in SharePoint or via code)
- Example (web.config):
  ```xml
  <appSettings>
    <add key="ClientId" value="<your-client-id>" />
    <add key="ClientSecret" value="<your-client-secret>" />
    <add key="RemoteServiceUrl" value="<remote-service-url>" />
  </appSettings>
  ```

## 4. Implement the Remote Event Receiver
- Add a Remote Event Receiver to your add-in project (e.g., via Visual Studio template).
- Implement the event handling logic in the RER service (e.g., `ProcessEvent`, `ProcessOneWayEvent`).
- Deploy the RER endpoint to a public HTTPS URL.

## 5. Deploy and Test
1. Publish your provider-hosted app and RER endpoint.
2. Deploy the add-in to your SharePoint site (via App Catalog or site-level add-in install).
3. Trigger the event (e.g., add/update/delete list item) and verify the RER is called.

## Notes
- **Do not use Azure AD registration for this scenario.**
- The client secret is managed in SharePoint (Azure ACS) via AppRegNew.aspx.
- Ensure your RER endpoint is accessible over HTTPS and the certificate is valid.

## Troubleshooting
- Ensure the app domain and redirect URI match exactly what was registered.
- Confirm permissions are granted via AppInv.aspx.

---
For more details, see [Microsoft Docs: Create a provider-hosted SharePoint Add-in](https://learn.microsoft.com/en-us/sharepoint/dev/sp-add-ins/create-provider-hosted-sharepoint-add-ins).
