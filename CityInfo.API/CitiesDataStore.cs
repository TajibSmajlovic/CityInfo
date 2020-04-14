using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Visoko",
                    Description = "Tamo gdje su piramide",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id= 1,
                            Name = "Piramida Sunca",
                            Description = "Jes piramida sto posto"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Sarajevo",
                    Description = "Tamo gdje je bila olimpijada",
                    PointOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id= 3,
                            Name = "Baščaršija",
                            Description = "Ko se jednom napije..."
                        }
                    }
                }
            };
        }
    }
}