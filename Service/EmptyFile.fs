// This file is here to prevent the following Exception:
//
// System.InvalidOperationException: A suitable constructor for type 'Giraffe.Middleware+GiraffeMiddleware'
// could not be located. Ensure the type is concrete and services are registered for all parameters of a
// public constructor.
//
// This only happens when the route handlers are in the main module, so this file is a quick fix to that.
