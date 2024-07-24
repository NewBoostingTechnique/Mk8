export default class HttpStatusCode {
  static get InternalServerError() { return 500; }

  // Not authorized.
  static get Forbidden() { return 403; }

  // Not authenticated.
  static get Unauthorized() { return 401; }
}