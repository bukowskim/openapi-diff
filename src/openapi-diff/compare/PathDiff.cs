﻿using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Compare;
using openapi_diff.utils;

namespace openapi_diff.compare
{
    public class PathDiff
    {
        private readonly OpenApiDiff _openApiDiff;

        public PathDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
        }

        public ChangedPathBO Diff(OpenApiPathItem left, OpenApiPathItem right, DiffContextBO context)
        {
            var oldOperationMap = left.Operations;
            var newOperationMap = right.Operations;
            var operationsDiff =
                MapKeyDiff<OperationType, OpenApiOperation>.Diff(oldOperationMap, newOperationMap);
            var sharedMethods = operationsDiff.SharedKey;
            var changedPath = new ChangedPathBO(context.URL, left, right, context)
            {
                Increased = operationsDiff.Increased,
                Missing = operationsDiff.Missing
            };
            foreach (var operationType in sharedMethods)
            {
                var oldOperation = oldOperationMap[operationType];
                var newOperation = newOperationMap[operationType];
                changedPath.Changed.Add(_openApiDiff
                        .OperationDiff
                        .Diff(oldOperation, newOperation, context.CopyWithMethod(operationType)));
            }

            changedPath.Extensions = _openApiDiff
                .ExtensionsDiff
                .Diff(left.Extensions, right.Extensions, context);

            return ChangedUtils.IsChanged(changedPath);
        }
    }
}
