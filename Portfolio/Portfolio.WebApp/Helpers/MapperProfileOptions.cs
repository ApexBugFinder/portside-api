using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAntWeb.API.Helpers
{
    public class MapperProfileOptions
    {
        public bool User { get; set; }
        public bool Allergy { get; set; }
        public bool Ingredient { get; set; }
        public bool Keyword { get; set; }
        public bool Recipe { get; set; }
        public bool RecipeAllergy { get; set; }
        public bool RecipeIngredient { get; set; }
        public bool RecipeKeyword { get; set; }
        public bool RecipeRecommendedBy { get; set; }
        public bool RecipeServes { get; set; }
        public bool RecipeSubmittedBy { get; set; }
        public bool Serves { get; set;  }
        public bool Step { get; set; }

        public MapperProfileOptions()
        {
            User = false;
            Allergy = false;
            Ingredient = false;
            Keyword = false;
            Recipe = false;
            RecipeAllergy = false;
            RecipeIngredient = false;
            RecipeKeyword = false;
            RecipeRecommendedBy = false;
            RecipeServes = false;
            RecipeSubmittedBy = false;
            Serves = false;
            Step = false;
        }
        
    }
}
