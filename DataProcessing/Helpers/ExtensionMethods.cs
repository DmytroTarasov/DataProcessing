using System.Collections.Generic;
using System.Linq;
using DataProcessing.Models;
using Newtonsoft.Json;

namespace DataProcessing.Helpers;

public static class ExtensionMethods
{
    public static IEnumerable<City> Transform(this IEnumerable<Payer> payers)
    {
        return payers
            .GroupBy(p => p.City)
            .Select(g1 =>
            {
                var cityServices = g1
                    .GroupBy(p => p.Service)
                    .Select(g2 => new Service
                    {
                        Name = g2.Key,
                        Payers = g2.ToList(),
                        Total = g2.Select(payer => payer.Payment).Sum()
                    }).ToList();

                return new City
                {
                    Name = g1.Key,
                    Services = cityServices,
                    Total = cityServices.Select(s => s.Total).Sum()
                };
            });
    }

    public static string ToJson(this IEnumerable<City> cities)
    {
        return JsonConvert.SerializeObject(cities);
    }
}