# GetContainerItems - Hierarchical Traversal Implementation

## Overview
GetContainerItems telah diubah untuk melakukan traversal hierarki lengkap, dimulai dari root container (ID: "0") dan mengambil semua child containers secara rekursif.

## Cara Kerja Hierarchical Traversal

### Flow Process
```
1. Start → GetContainerItems(ID: "0")
           ↓
2. Response berisi ContainerItem dengan child items
           ↓
3. Extract semua child ContainerItem IDs
           ↓
4. Untuk setiap child ID:
   → GetContainerItems(child ID)
   → Ulangi langkah 2-4 (recursive)
           ↓
5. Build complete hierarchy tree
           ↓
6. Display dengan format hierarki
```

### Request Format
Sesuai dengan requirement, request dimulai dengan ID = "0":

```xml
<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" 
               xmlns:ns="http://www.schneider-electric.com/common/dataexchange/2011/05">
   <soap:Header/>
   <soap:Body>
      <ns:GetContainerItemsRequest version="2" metadata="false">
         <ns:GetContainerItemsIds>
            <ns:Id>0</ns:Id>
         </ns:GetContainerItemsIds>
         <ns:GetContainerItemsParameter>
         </ns:GetContainerItemsParameter>
      </ns:GetContainerItemsRequest>
   </soap:Body>
</soap:Envelope>
```

### Response Structure
Response berisi ContainerItem dengan nested children:

```xml
<GetContainerItemsResponse version="2">
   <GetContainerItemsItems>
      <ContainerItem>
         <Id>0</Id>
         <Name>Root</Name>
         <Description>Root</Description>
         <Type>server</Type>
         <Items>
            <ContainerItems>
               <ContainerItem>
                  <Id>System@GC</Id>
                  <Name>System</Name>
                  <Description>System</Description>
                  <Type>folder</Type>
               </ContainerItem>
            </ContainerItems>
         </Items>
      </ContainerItem>
   </GetContainerItemsItems>
</GetContainerItemsResponse>
```

## Implementation Details

### 1. Model Changes
**ContainerItemDto** sekarang mendukung struktur hierarki:

```csharp
public class ContainerItemDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public bool IsContainer { get; set; }
    public List<ContainerItemDto> Children { get; set; } = new();
    public int Level { get; set; } = 0; // For display indentation
}
```

### 2. Service Changes
**GetContainerItemsService** dengan recursive traversal:

```csharp
// Main method - starts from root
public async Task ExecuteAsync(string version, ...)
{
    var rootItem = await FetchContainerHierarchyAsync(version, "0", 0);
    // Display hierarchical structure
}

// Recursive fetch method
private async Task<ContainerItemDto?> FetchContainerHierarchyAsync(
    string version, string containerId, int level)
{
    // 1. Fetch current container
    var response = await GetContainerItemsAsync(...);
    
    // 2. Extract child container IDs
    var childContainerIds = ExtractChildIds(response);
    
    // 3. Recursively fetch each child
    foreach (var childId in childContainerIds)
    {
        var child = await FetchContainerHierarchyAsync(version, childId, level + 1);
        containerItem.Children.Add(child);
    }
    
    return containerItem;
}
```

### 3. Display Changes
**DisplayHelper** dengan hierarchical view:

```csharp
public static void DisplayContainerItemsHierarchical(...)
{
    foreach (var item in response.Items)
    {
        DisplayContainerItemRecursive(item, 0);
    }
}

private static void DisplayContainerItemRecursive(ContainerItemDto item, int level)
{
    string indent = new string(' ', level * 2);
    string branch = level > 0 ? "└─ " : "";
    
    Console.WriteLine($"{indent}{branch}📁 {item.Name}");
    Console.WriteLine($"{indent}   ID: {item.Id}");
    
    // Recursively display children
    foreach (var child in item.Children)
    {
        DisplayContainerItemRecursive(child, level + 1);
    }
}
```

## Example Output

### During Traversal
```
🔍 Memulai traversal hierarki dari root container (ID: 0)...

📂 Fetching container: 0 (level 0)
  📂 Fetching container: System@GC (level 1)
    📂 Fetching container: Alarms (level 2)
    📂 Fetching container: Config (level 2)
  📂 Fetching container: Data@GC (level 1)
```

### Final Display
```
================================================================================
HASIL RESPONSE - HIERARCHICAL VIEW:
================================================================================

Response Version: 2

================================================================================
CONTAINER HIERARCHY:
================================================================================

📁 Root
   ID: 0
   Description: Root
   Type: server
   Children: 1

  └─ 📁 System
     ID: System@GC
     Description: System
     Type: folder
     Children: 2

    └─ 📁 Alarms
       ID: System@GC/Alarms
       Type: folder
       Children: 0

    └─ 📁 Config
       ID: System@GC/Config
       Type: folder
       Children: 0

================================================================================
✓ Total items ditemukan: 4
================================================================================
```

## Benefits

### 1. Complete Hierarchy View
- Melihat seluruh struktur container dari root ke leaf
- Tidak perlu manual fetch setiap level
- Otomatis traverse semua children

### 2. Better Understanding
- Visual hierarchy dengan indentation
- Tree structure yang jelas
- Count children di setiap level

### 3. Easier Navigation
- Lihat semua paths dalam satu view
- Identifikasi structure dengan cepat
- Temukan nested containers dengan mudah

### 4. Progress Feedback
- Real-time feedback saat fetching
- Tahu level mana yang sedang diproses
- Debug-friendly jika ada error

## Technical Notes

### Performance Considerations
- Recursive calls bisa banyak jika hierarchy dalam
- Setiap level = 1 SOAP request
- Total requests = jumlah total containers dalam hierarchy

### Error Handling
- Error di satu branch tidak stop keseluruhan
- Menampilkan warning untuk container yang gagal
- Continue dengan containers lain

### Limitations
- Hanya traverse ContainerItems (tidak termasuk ValueItems, HistoryItems, etc.)
- Fokus pada struktur container hierarchy
- Metadata tidak diambil (metadata=false)

## Usage Example

```csharp
// Simply call - akan otomatis start dari root dan traverse semua
var service = new GetContainerItemsService(dataExchangeService);
await service.ExecuteAsync(version);

// Output:
// - Complete hierarchy tree
// - All containers dari root ke leaf
// - Progress feedback saat fetching
// - Summary total items
```

## Comparison: Before vs After

### Before
- Single call dengan satu container ID
- Flat list display
- Manual iteration untuk child containers
- User harus tahu ID containers

### After
- Auto start dari root (ID: "0")
- Recursive traversal seluruh hierarchy
- Hierarchical tree display
- Complete view tanpa manual work

## Future Enhancements (Optional)

1. **Depth Limit**: Add parameter untuk limit traversal depth
2. **Filtering**: Filter containers by type atau pattern
3. **Caching**: Cache results untuk avoid duplicate requests
4. **Parallel Fetch**: Fetch siblings in parallel untuk speed up
5. **Export**: Export hierarchy ke JSON/XML file

## Troubleshooting

### Issue: "No items found"
- Check server mendukung GetContainerItems
- Verify ID "0" adalah valid root container
- Check permissions/credentials

### Issue: "Slow performance"
- Normal jika hierarchy dalam
- Consider menambah timeout
- Monitor jumlah requests

### Issue: "Partial hierarchy"
- Mungkin ada errors di beberapa branches
- Check console untuk warnings
- Verify all container IDs valid
