using System;

namespace Forms.Models.NewModels
{
  public class NewResponseViewModel
  {
    public string formId { get; set; }

    public string createdBy { get; set; }

    public NewResponseValuesViewModel[] responseValues { get; set; }
  }
}