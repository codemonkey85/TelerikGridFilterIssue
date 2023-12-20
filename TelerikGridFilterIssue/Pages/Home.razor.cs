namespace TelerikGridFilterIssue.Pages;

// ReSharper disable once UnusedType.Global
public partial class Home
{
    private TelerikGrid<GridDataItem>? Grid { get; set; }

    private List<GridDataItem> GridData { get; set; } = new();

    private async Task Load()
    {
        GridData.Clear();
        GridData.AddRange(Enumerable.Range(1, 100).Select(i => new GridDataItem
        {
            Name = $"Test {i}",
            RegionId = (RegionsByType)(i % 4),
            StatusId = (CustStatus)(i % 4),
        }));

        await BuildDefaultFilterDescriptors();

        StateHasChanged();
    }

    private Task BuildDefaultFilterDescriptors()
    {
        if (Grid is null)
        {
            return Task.CompletedTask;
        }

        var gridState = Grid.GetState();
        gridState.FilterDescriptors.Clear();

        var filterDescriptorCollection = new FilterDescriptorCollection
        {
            new FilterDescriptor
            {
                Member = nameof(GridDataItem.RegionId),
                Operator = FilterOperator.IsEqualTo,
                Value = RegionsByType.Americas,
                MemberType = typeof(RegionsByType),
            },
            new FilterDescriptor
            {
                Member = nameof(GridDataItem.StatusId),
                Operator = FilterOperator.IsNotEqualTo,
                Value = CustStatus.Expired,
                MemberType = typeof(CustStatus),
            }
        };

        gridState.FilterDescriptors.Add(
            new CompositeFilterDescriptor
            {
                FilterDescriptors = filterDescriptorCollection,
                LogicalOperator = FilterCompositionLogicalOperator.And,
            }
        );

        return Grid.SetStateAsync(gridState);
    }

    private class GridDataItem
    {
        public string? Name { get; set; }

        public RegionsByType RegionId { get; set; }

        public CustStatus StatusId { get; set; }
    }

    private enum RegionsByType
    {
        Americas,
        AsiaPacific,
        Emea,
        Other,
    }

    private enum CustStatus
    {
        UnQual999 = 0,
        Active = 1,
        Inactive = 2,
        Expired = 3,
    }

    private static string ToDisplayText(RegionsByType @enum) =>
        @enum switch
        {
            RegionsByType.Americas => "Americas",
            RegionsByType.AsiaPacific => "Asia/Pacific",
            RegionsByType.Emea => "EMEA",
            RegionsByType.Other => "Other",
            _ => $"*Unmatched RegionsByType Value {@enum}*"
        };

    private static string ToDisplayText(CustStatus @enum) =>
        @enum switch
        {
            CustStatus.UnQual999 => "UnQual/999",
            CustStatus.Active => "Active",
            CustStatus.Inactive => "Inactive",
            CustStatus.Expired => "Expired",
            _ => $"*Unmatched CustStatus Value {@enum}*"
        };
}
