using ArielWebRecipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ArielWebRecipe.WebApi.Models
{
    public class PreparationStepInfo
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public string Description { get; set; }

        public int PreparationTime { get; set; }

        public string PictureLink { get; set; }

        public static Expression<Func<PreparationStep, PreparationStepInfo>> FromPreparationStep
        {
            get
            {
                return x => new PreparationStepInfo
                {
                    Id = x.Id,
                    Order = x.Order,
                    Description = x.Description,
                    PreparationTime = x.PreparationTime,
                    PictureLink=x.PictureLink
                };
            }
        }
    }
}