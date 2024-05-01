namespace ProjSecDemo.Data
{
    public class Product
    {
            public int id { get; set; } 
            public string Titre { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Fabricant { get; set; } = string.Empty;
            public string? InfoSup { get; set; } = string.Empty;
            public int Prix_Vente { get; set; } 
            public string Type{ get; set; } = string.Empty;

    }
}

