namespace EmployeeHierarchy.Domain.Entities;
public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ManagerId { get; set; }
    public List<Employee> Reports { get; set; } = new();
}