﻿using System;
using FluentValidation;
using GraphQL.FluentValidation;

namespace GraphQL
{
    public static partial class FluentValidationExtensions
    {
        /// <summary>
        /// Wraps <see cref="ResolveFieldContextExtensions.GetArgument{TType}"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.Validate(IValidationContext)"/> to perform validation.
        /// If a <see cref="ValidationException"/> occurs it will be converted to <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static TArgument GetValidatedArgument<TArgument>(this IResolveFieldContext context, string name, TArgument defaultValue = default)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(name, defaultValue);
            var validatorCache = context.GetCache();
            ArgumentValidation.Validate<TArgument>(validatorCache, typeof(TArgument), argument, context.UserContext, context.Schema as IServiceProvider);
            return argument;
        }

        /// <summary>
        /// Wraps <see cref="ResolveFieldContextExtensions.GetArgument{TType}"/> to validate the resulting argument instance.
        /// Uses <see cref="IValidator.Validate(IValidationContext)"/> to perform validation.
        /// If a <see cref="ValidationException"/> occurs it will be converted to <see cref="ExecutionError"/>s by a field middleware.
        /// </summary>
        public static object GetValidatedArgument(this IResolveFieldContext context, Type argumentType, string name, object? defaultValue = null)
        {
            Guard.AgainstNull(context, nameof(context));
            var argument = context.GetArgument(argumentType, name, defaultValue);
            var validatorCache = context.GetCache();
            ArgumentValidation.Validate(validatorCache, argumentType, argument, context.UserContext, context.Schema as IServiceProvider);
            return argument;
        }
    }
}