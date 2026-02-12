# Known Limitations and Workarounds

## GetItems Limitation

### Issue
The `GetItems` operation requires a list of specific Item IDs to be provided. When called without IDs (option 4 in the menu), the server returns a `MISSING_ID_LIST` error.

### Why This Happens
The PME DataExchange service requires that `GetItems` be called with specific item IDs. Unlike `GetContainerItems` which can list items in a container, `GetItems` is designed to retrieve detailed information about already-known items.

### Workaround

**Recommended Approach:**
1. First, use **GetContainerItems** (option 6) to discover available items
2. Note the IDs of the items you're interested in
3. Modify the code to pass those IDs to GetItems

**Example Code Modification:**
```csharp
static async Task RunGetItems(DataExchangeService dataExchangeService, string version)
{
    var service = new GetItemsService(dataExchangeService);
    
    // Instead of null, provide specific item IDs
    var itemIds = new List<string> 
    { 
        "your-item-id-1",
        "your-item-id-2"
    };
    
    await service.ExecuteAsync(version, itemIds, false);
}
```

### Alternative Operations

If you just want to browse available items, use these operations instead:

- **GetContainerItems** (option 6) - Browse items in containers, works without IDs
- **GetWebServiceInformation** (option 1) - See what operations are available
- **GetValues** (option 5) - Get current values if you already have item IDs

### Current Behavior

When you select option 4 (GetItems), the application will:
1. Attempt to call GetItems without item IDs
2. Receive MISSING_ID_LIST error from server
3. Display a helpful message explaining the limitation
4. Suggest using GetContainerItems instead
5. Continue execution without crashing

This graceful error handling ensures you understand why GetItems needs customization before it can be used effectively.

## GetValues Limitation

Similar to GetItems, `GetValues` also requires specific item IDs. The same workaround applies - use GetContainerItems first to discover item IDs, then customize the code to pass those IDs.

## Summary

For operations that require item IDs:
1. **Discovery phase**: Use GetContainerItems to find available items
2. **Customization phase**: Update the code with specific IDs
3. **Query phase**: Run GetItems or GetValues with the IDs

This is by design in the PME DataExchange API - these operations are meant for targeted queries, not for browsing.
