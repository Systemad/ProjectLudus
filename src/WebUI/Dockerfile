FROM node:22 AS builder

WORKDIR /app
COPY package.json package-lock.json ./

#RUN corepack enable
RUN npm install
COPY . .
RUN npm run build

COPY --from=builder /app/build/ .
