FROM nginx:alpine
COPY index.html config.template.js /usr/share/nginx/html/
COPY 40-config.sh /docker-entrypoint.d/40-config.sh
RUN chmod +x /docker-entrypoint.d/40-config.sh