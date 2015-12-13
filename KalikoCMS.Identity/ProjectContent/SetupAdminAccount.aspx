<%@ Page Language="C#" %>
<%@ Import Namespace="AspNet.Identity.DataAccess" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<%@ Import Namespace="KalikoCMS.Identity" %>
<%
  // To create a basic admin account uncomment the code below and change the password and email, then run the page.
  // This file should be deleted immediately after!!
  
  /*
  var userName = "admin";
  var password = "";
  var email = "admin@example.com";
  var roleName = "WebAdmin";

  var roleManager = new RoleManager<IdentityRole, Guid>(new RoleStore());
  var role = roleManager.FindByName(roleName);

  if (string.IsNullOrEmpty(password)) {
      throw new Exception("You need to set a secure password!");
  }

  if (role == null) {
      role = new IdentityRole(roleName);
      roleManager.Create(role);
  }

  var userManager = IdentityUserManager.GetManager();

  var user = userManager.FindByName(userName);

  if (user == null) {
      user = new IdentityUser(userName) { Email = email };
      var result = userManager.Create(user, password);
      if (!result.Succeeded) {
          throw new Exception("Could not create user due to: " + string.Join(", ", result.Errors));
      }
  }

  userManager.AddToRole(user.Id, roleName);

  Response.Write("Role and user created!");
  */
%>