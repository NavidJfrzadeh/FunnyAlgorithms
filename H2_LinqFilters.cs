//filter some data from ViewModel
public class Search
{
    public List<Service> GetServices(ServiceViewModel searchModel)
    {
        var services = MyMemory.Services;

        if (searchModel.Date != null && searchModel.Gender != null)
        {
            var dayOftheWeek = searchModel.Date.Value.DayOfWeek;
            services = MyMemory.Experts.Where(p => p.AvailableDays.Contains(dayOftheWeek) && p.Gender == searchModel.Gender).SelectMany(p => p.Services).ToList();
        }

        else if (searchModel.Date != null)
        {
            var dayOftheWeek = searchModel.Date.Value.DayOfWeek;
            services = MyMemory.Experts.Where(p => p.AvailableDays.Contains(dayOftheWeek)).SelectMany(p => p.Services).ToList();
        }

        else if (searchModel.Gender != null)
        {
            services = MyMemory.Experts.Where(q => q.Gender == searchModel.Gender).SelectMany(p => p.Services).ToList();
        }


        if (searchModel.ServiceName != null)
            services = services.Where(x => x.Name.Contains(searchModel.ServiceName)).ToList();

        if (searchModel.Location != null)
            services = services.Where(x => x.Location.Contains(searchModel.Location)).ToList();

        if (searchModel.Title != null)
            services = services.Where(x => x.Categories.Select(p => p.Title).Contains(searchModel.Title)).ToList();




        return services;
    }
}