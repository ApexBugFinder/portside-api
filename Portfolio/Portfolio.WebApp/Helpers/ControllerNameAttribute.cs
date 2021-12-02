using System;

namespace Portfolio.WebApp.Helpers {
[AttributeUsage(AttributeTargets.Class)]


  public class ControllerNameAttribute : Attribute {


public string Name { get; }

public ControllerNameAttribute(string name) {
  Name = name;
}
  }
}