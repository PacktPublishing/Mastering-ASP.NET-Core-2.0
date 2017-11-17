using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.WebComponents
{
    public class BoolModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(bool) || bindingContext.ModelType == typeof(bool?))
            {
                var modelName = bindingContext.BinderModelName ?? bindingContext.ModelName;

                var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

                if (valueProviderResult == ValueProviderResult.None)
                {
                    return Task.CompletedTask;
                }

                var value = valueProviderResult.FirstValue;

                if (string.IsNullOrWhiteSpace(value) == true)
                {
                    return Task.CompletedTask;
                }

                if (bool.TryParse(value, out bool result) == true)
                {
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
            }

            return Task.CompletedTask;
        }
    }
}
