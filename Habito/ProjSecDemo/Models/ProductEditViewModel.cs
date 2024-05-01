namespace ProjSecDemo.Models
{
    public class ProductEditViewModel
    {
       
            public int id { get; set; }
            public string Titre { get; set; } 
            public string Description { get; set; } 
            public string Fabricant { get; set; } 
            public string InfoSup { get; set; } 
            public int Prix_Vente { get; set; }
            public string Type { get; set; }
            public bool DroitEdit { get; set; }

    }

}
