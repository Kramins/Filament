using filament.data;
using filament.data.models;

namespace filament.services;

public class LibraryService
{

    FilamentDataContext _dataContext;


    public LibraryService(FilamentDataContext filamentDataContext)
    {
        _dataContext = filamentDataContext;
    }

    public IEnumerable<Library> GetAllWithBasic()
    {
        return _dataContext.Libraries.AsQueryable();
    }

    public int Add(Library library)
    {
        _dataContext.Libraries.Add(library);
        _dataContext.SaveChanges();
        return library.Id;
    }
}