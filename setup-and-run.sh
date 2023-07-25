git switch develop
git pull
docker container stop apicadastro
docker container rm -f apicadastro
docker build  -t cadastro-api .
docker run -d -p 3005:80 --name apicadastro cadastro-api