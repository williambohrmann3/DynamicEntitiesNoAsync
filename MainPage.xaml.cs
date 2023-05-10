using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.RealTime;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace DynamicEntitiesNoAsync;
public partial class MainPage : ContentPage
{

    private Envelope _utahSandyEnvelope = new Envelope(new MapPoint(-112.110052, 40.718083, SpatialReferences.Wgs84), new MapPoint(-111.814782, 40.535247, SpatialReferences.Wgs84));

    private ArcGISStreamService _dynamicEntityDataSource;
    private DynamicEntityLayer _dynamicEntityLayer;

    public MainPage()
    {
        InitializeComponent();
        ArcGISRuntimeEnvironment.ApiKey = "your API key here";

        this.BindingContext = new MapViewModel();
        Initialize();
    }

    private void Initialize()
    {
        MyMapView.Map = new Map(BasemapStyle.ArcGISDarkGrayBase);

        // Create the stream service. A stream service is one type of dynamic entity data source.
        var streamServiceUrl = "https://realtimegis2016.esri.com:6443/arcgis/rest/services/SandyVehicles/StreamServer";
        _dynamicEntityDataSource = new ArcGISStreamService(new Uri(streamServiceUrl));

        // Add a filter for the data source, to limit the amount of data received by the application.
        _dynamicEntityDataSource.Filter = new ArcGISStreamServiceFilter()
        {
            // Filter with an envelope.
            Geometry = _utahSandyEnvelope,

            // Use a where clause to filter by attribute values.
            WhereClause = "speed > 0"
        };

        _dynamicEntityLayer = new DynamicEntityLayer(_dynamicEntityDataSource);

        // Add the dynamic entity layer to the map.
        MyMapView.Map.OperationalLayers.Add(_dynamicEntityLayer);
    }
}

