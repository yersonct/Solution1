using AutoMapper;
using Entity.Model; // Asegúrate de que el namespace de tus modelos sea correcto
using Entity.DTOs; // Asegúrate de que el namespace de tus DTOs sea correcto

namespace Business.MapperProfiles // Ajusta este namespace a la ubicación de tu proyecto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -------------------------------------------------------------
            // Mapeos de Entidad (Model) a DTO (para respuestas de API)
            // -------------------------------------------------------------

            // Mapeo User a UserDTO
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.PersonName, // PersonName en UserDTO no existe directamente en User
                           opt => opt.MapFrom(src => src.Person != null ? $"{src.Person.Name} {src.Person.Lastname}" : null)); // Lo construimos desde la propiedad de navegación Person

            // Mapeo RolUser a RolUserDTO
            CreateMap<RolUser, RolUserDTO>()
                .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol != null ? src.Rol.Name : null)) // Obtenemos el nombre del Rol a través de la propiedad de navegación
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Username : null)); // Obtenemos el nombre de usuario a través de la propiedad de navegación

            // Mapeo de Entidades simples a DTOs (donde los nombres de propiedades son idénticos o auto-inferibles)
            CreateMap<Rol, RolDTO>();
            CreateMap<Person, PersonDTO>();
            CreateMap<Permission, PermissionDTO>();
            CreateMap<Modules, ModuleDTO>(); // Clase 'Modules' en Model a 'ModuleDTO'

            CreateMap<Forms, FormDTO>();

            // Mapeo FormRolPermission a FormRolPermissionDTO
            CreateMap<FormRolPermission, FormRolPermissionDTO>()
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Forms != null ? src.Forms.Name : null))
                .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol != null ? src.Rol.Name : null))
                .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission != null ? src.Permission.Name : null));

            // Mapeo FormModule a FormModuleDTO
            CreateMap<FormModule, FormModuleDTO>()
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Forms != null ? src.Forms.Name : null))
                .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.Modules != null ? src.Modules.Name : null));


            // -------------------------------------------------------------
            // Mapeos de DTO (Create/Update) a Entidad (Model)
            // -------------------------------------------------------------

            // Mapeo UserCreateDTO a User
            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // El password se hashea en el servicio, no se mapea directamente
                                                                       //.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow)) // Si tu modelo User tiene una propiedad CreatedDate
                .ForMember(dest => dest.Person, opt => opt.Ignore()) // No mapear la entidad completa de Person desde DTO de creación
                .ForMember(dest => dest.RolUsers, opt => opt.Ignore()); // La colección RolUsers no se mapea directamente al crear un User

            // Mapeo RolUserCreateDTO a RolUser
            CreateMap<RolUserCreateDTO, RolUser>()
                .ForMember(dest => dest.User, opt => opt.Ignore()) // No mapear la entidad User
                .ForMember(dest => dest.Rol, opt => opt.Ignore()); // No mapear la entidad Rol

            // Mapeo FormRolPermissionCreateDTO a FormRolPermission
            CreateMap<FormRolPermissionCreateDTO, FormRolPermission>()
                .ForMember(dest => dest.Forms, opt => opt.Ignore())
                .ForMember(dest => dest.Rol, opt => opt.Ignore())
                .ForMember(dest => dest.Permission, opt => opt.Ignore());

            // Mapeo FormModuleCreateDTO a FormModule
            CreateMap<FormModuleCreateDTO, FormModule>()
                .ForMember(dest => dest.Forms, opt => opt.Ignore())
                .ForMember(dest => dest.Modules, opt => opt.Ignore());

            // Si tienes DTOs para crear/actualizar otras entidades, agrégalos aquí:
            // Ejemplo: CreateMap<PersonCreateDTO, Person>();
            // Ejemplo: CreateMap<RolCreateDTO, Rol>();
            // ...
        }
    }
}