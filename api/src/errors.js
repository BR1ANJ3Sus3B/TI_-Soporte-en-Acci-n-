export class ApiError extends Error {
  constructor(status, code, message) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.code = code;
  }
}

export function errorHandler(error, _request, response, _next) {
  if (response.headersSent) return;
  if (error?.type === 'entity.too.large') {
    response.status(413).json({ error: { code: 'payload_too_large', message: 'El cuerpo supera el límite permitido.' } });
    return;
  }
  if (error instanceof SyntaxError && Number.isInteger(error.status) && error.status === 400) {
    response.status(400).json({ error: { code: 'invalid_json', message: 'El cuerpo JSON no es válido.' } });
    return;
  }
  if (error?.code === 11000) {
    response.status(409).json({ error: { code: 'duplicate_resource', message: 'El recurso ya existe.' } });
    return;
  }
  if (error?.name === 'ValidationError' || error?.name === 'CastError') {
    response.status(400).json({ error: { code: 'invalid_data', message: 'Los datos enviados no son válidos.' } });
    return;
  }
  if (error instanceof ApiError) {
    response.status(error.status).json({ error: { code: error.code, message: error.message } });
    return;
  }
  console.error('Unhandled API error:', error?.name ?? 'Error');
  response.status(500).json({ error: { code: 'internal_error', message: 'Ocurrió un error interno.' } });
}
