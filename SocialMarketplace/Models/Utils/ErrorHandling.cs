using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.Utils
{
    public static class ErrorHandling
    {
        public static void AddModelError(ModelStateDictionary model, Exception ex)
        {
            if(ex is DbEntityValidationException)
            {
                String msg = string.Empty;
                var e = ex as DbEntityValidationException;
                foreach (var eve in e.EntityValidationErrors)
                {
                    msg += $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors: ";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        msg += $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"";
                    }

                    model.AddModelError(string.Empty, ex.Message + " " + msg);
                }
            }
            else
                model.AddModelError(string.Empty, ex.Message + " " + ex.StackTrace.ToString());
        }
    }
}