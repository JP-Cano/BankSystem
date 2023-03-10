using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Helpers.ObjectsUtils
{
    [ExcludeFromCodeCoverage]
    public static class MapperObject
    {
        public static IMapper mapper = new MapperConfiguration(cfg => { }).CreateMapper();
        public static IMapper mapperWithConstructor = new MapperConfiguration(cfg => { cfg.ShouldUseConstructor = ci => !ci.IsPrivate; }).CreateMapper();
    }
}