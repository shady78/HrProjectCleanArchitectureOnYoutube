using HRManagement.Application.Common;

namespace HRManagement.Application.Services.Departments
{
    public static class DepartmentErrors
    {
        public static ServiceError NotFound(int id)
            => new(

              "Departments.NotFound",

              $"Department with id '{id}' was not found.",

              ErrorType.NotFound);



        public static ServiceError DuplicateName(string name)
              => new(

                "Departments.DuplicateName",

                $"A department named '{name}' already exists.",

                ErrorType.Conflict);

    }
}
