import 'dotenv/config';
import { createApp } from './app.js';
import { connectDatabase, disconnectDatabase } from './config/database.js';
import { env } from './config/env.js';

let server;

async function start() {
  await connectDatabase(env.mongoUri);
  server = createApp().listen(env.port, '0.0.0.0', () => {
    console.log(`TI Soporte API listening on port ${env.port}`);
  });
}

async function shutdown(signal) {
  console.log(`Received ${signal}; shutting down API.`);
  if (server) await new Promise((resolve) => server.close(resolve));
  await disconnectDatabase();
  process.exit(0);
}

process.once('SIGINT', () => { void shutdown('SIGINT'); });
process.once('SIGTERM', () => { void shutdown('SIGTERM'); });

start().catch((error) => {
  console.error(`API startup failed: ${error.message}`);
  process.exitCode = 1;
});
