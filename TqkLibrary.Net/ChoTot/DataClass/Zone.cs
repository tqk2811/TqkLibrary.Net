﻿namespace TqkLibrary.Net.ChoTot
{
  public class Zone
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public override string ToString()
    {
      return $"{Id}: {Name}";
    }
  }
}