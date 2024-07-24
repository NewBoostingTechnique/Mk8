export default class ApiError extends Error {
  constructor(message, traceId) {
    super(message);
    this.traceId = traceId;
  }
}