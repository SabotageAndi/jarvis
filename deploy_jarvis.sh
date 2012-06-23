#/bin/sh
rm -R /var/www/jarvis/*
cp -R /home/andreas/jarvis/* /var/www/jarvis
chown -R www-data:www-data /var/www/jarvis/*
rm /var/www/jarvis/bin/Microsoft.*
rm /var/www/jarvis/bin/WebActivator.*
#mv /var/www/jarvis/Web.config /var/www/jarvis/web.config
cp ninject/* /var/www/jarvis/bin/
cp ninject.wcf/Ninject.Extensions.Wcf.* /var/www/jarvis/bin/
cp npgsql/Npgsql2.0.11.93/Mono2.0/bin/Npgsql.dll /var/www/jarvis/bin/
cp npgsql/Npgsql2.0.11.93/Mono2.0/bin/Npgsql.xml /var/www/jarvis/bin/

mkdir /var/www/jarvis/Addins
service apache2 restart
