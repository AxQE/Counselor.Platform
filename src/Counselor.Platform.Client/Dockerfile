FROM nginx:1.21.4-alpine
COPY ./build /var/www
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80 30080
EXPOSE 443 30443

ENTRYPOINT ["nginx", "-g", "daemon off;"]