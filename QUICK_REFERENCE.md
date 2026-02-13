# Quick Reference - Menu Organization

## Menu Sections

### ✓ SUPPORTED OPERATIONS (Menu 1-9)
Operations confirmed by GetWebServiceInformation response from your PME server.

| Menu | Operation | Complexity | Notes |
|------|-----------|------------|-------|
| 1 | GetWebServiceInformation | Simple | ROOT - Start here! |
| 2 | GetAlarmEventTypes | Simple | Only needs version |
| 3 | GetContainerItems | Simple | Use this to explore! |
| 4 | GetAlarmEvents | Simple | Active alarm events |
| 5 | GetUpdatedAlarmEvents | Simple | Updated alarms |
| 6 | AcknowledgeAlarmEvents | Simple | Acknowledge alarms |
| 7 | GetItems | Medium | Requires Item IDs |
| 8 | GetValues | Medium | Requires Item IDs |
| 9 | GetHistory | Complex | More parameters |

### ✗ UNSUPPORTED OPERATIONS (Menu 10-22)
These operations are NOT in your server's supported list. They will likely return `OPERATION_NOT_SUPPORTED` error.

| Menu | Operation | Expected Result |
|------|-----------|-----------------|
| 10 | GetEnums | ❌ Likely unsupported |
| 11 | ForceValues | ❌ Likely unsupported |
| 12 | GetAlarmHistory | ❌ Likely unsupported |
| 13 | GetHierarchicalInformation | ❌ Likely unsupported |
| 14 | GetHistoricalDataAggregation | ❌ Likely unsupported |
| 15 | GetNotification | ❌ Likely unsupported |
| 16 | GetSystemEvents | ❌ Likely unsupported |
| 17 | GetSystemEventTypes | ❌ Likely unsupported |
| 18 | Renew | ❌ Likely unsupported |
| 19 | SetValues | ❌ Likely unsupported |
| 20 | Subscribe | ❌ Likely unsupported |
| 21 | UnforceValues | ❌ Likely unsupported |
| 22 | Unsubscribe | ❌ Likely unsupported |

### Menu 23 - Run All Supported
Executes ONLY the supported operations (menu 1-9), not all 22 operations.

## Recommended Workflow

```
START → 1. GetWebServiceInformation (verify connection)
         ↓
        3. GetContainerItems (explore structure, get IDs)
         ↓
        2. GetAlarmEventTypes (check alarm types)
         ↓
        4-9. Use other operations as needed
```

## Visual Indicators

- **✓** = Supported by server (confirmed via GetWebServiceInformation)
- **✗** = NOT in supported operations list (will likely fail)

## Benefits

1. **Clear Organization** - Immediately see what works
2. **Time Saving** - Focus on working operations
3. **Sorted by Complexity** - Simple operations first
4. **Better UX** - No trial and error needed
5. **Smart Run All** - Only runs supported operations
