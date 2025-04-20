using UriShortener.Data.Core;

namespace UriShortener.Data.Entities;

public class Role
{
  public Role(){}
  public Role(UserRole role){
    Id = (int)role;
    Title = role;
  }

  public int Id { get; set; }
  public UserRole Title { get; set; }
  public ICollection<User> Users { get; set; } = new List<User>();
  public ICollection<UsersRoles> UsersRoles { get; set; } = new List<UsersRoles>();
}