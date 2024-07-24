import ApiError from "../Errors/ApiError";
import HttpStatusCode from "../Http/HttpStatusCode";

export default class ApiClient {
  constructor() {
    if (this.constructor === ApiClient) {
      throw new Error(`Cannot instantiate abstract class ApiClient`);
    }
  }

  async fetchAsync(...args) {
    let [resource, config] = args;
    const response = await fetch(resource, config);

    if (response.status === HttpStatusCode.InternalServerError) {
      const data = await response.json();
      throw new ApiError(data.title, data.traceId);
    }

    return response;
  };
}