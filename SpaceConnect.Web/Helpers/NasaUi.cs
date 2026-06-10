namespace SpaceConnect.Web.Helpers;

public static class NasaUi
{
    private static readonly Dictionary<string, string> CategoryImages = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Saúde"] = "/images/cat-saude.jpg",
        ["Agricultura"] = "/images/cat-agricultura.jpg",
        ["Consumo"] = "/images/cat-consumo.jpg",
        ["Comunicação"] = "/images/cat-comunicacao.jpg",
        ["Energia"] = "/images/cat-energia.jpg",
        ["Segurança"] = "/images/cat-seguranca.jpg",
        ["Transporte"] = "/images/cat-transporte.jpg",
        ["Meio Ambiente"] = "/images/cat-meio-ambiente.jpg"
    };

    private static readonly Dictionary<int, string> SpinoffImages = new()
    {
        [1] = "/images/spinoff-espuma.jpg",
        [2] = "/images/spinoff-cmos.jpg",
        [3] = "/images/spinoff-agua.jpg",
        [4] = "/images/spinoff-cardio.jpg",
        [5] = "/images/spinoff-satelite.jpg",
        [6] = "/images/spinoff-solar.jpg",
        [7] = "/images/spinoff-ferramentas.jpg",
        [8] = "/images/spinoff-isolamento.jpg",
        [9] = "/images/spinoff-gps.jpg",
        [10] = "/images/spinoff-alimentos.jpg"
    };

    public const string HeroImage = "/images/hero.jpg";
    public const string HeroVideo = "/video/hero-earth.mp4";
    public static string HeroVideoUrl => $"{HeroVideo}?v=9";
    public const string AuthHeroImage = "/images/auth.jpg";
    private const string ImgVer = "v=10";

    public static string HeroStyle(string? url = null) =>
        $"background-image: url('{url ?? HeroImage}?{ImgVer}');";

    public static string CategoryImage(string? categoria) =>
        categoria != null && CategoryImages.TryGetValue(categoria, out var img)
            ? $"{img}?{ImgVer}"
            : $"/images/cat-default.jpg?{ImgVer}";

    public static string SpinoffImage(int id, string? categoria = null) =>
        SpinoffImages.TryGetValue(id, out var img) ? $"{img}?{ImgVer}" : CategoryImage(categoria);

    public static string CategoryIcon(string? icone) => icone?.ToLowerInvariant() switch
    {
        "heart" => "bi-heart-pulse",
        "leaf" => "bi-tree",
        "shopping-bag" => "bi-bag",
        "radio" => "bi-broadcast",
        "zap" => "bi-lightning-charge",
        "shield" => "bi-shield-check",
        "rocket" => "bi-rocket-takeoff",
        "globe" => "bi-globe-americas",
        _ => "bi-circle"
    };
}
