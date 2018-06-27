using System;

namespace Forms.Models.NewModels
{
  public class NewFormViewModel
  {
    public string createdBy { get; set; }

    public string formTitle { get; set; }

    public NewFieldViewModel[] fields { get; set; }
  }
}