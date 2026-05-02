pipeline {
    agent any
    
    stages {
        stage('Deploy backend') {
            steps {
                dir('/home/malde/projects/BackEnd') {
                    sh 'git fetch origin main && git reset --hard origin/main'
                }
                
                dir('/home/malde/projects/infrastructure') {
                    sh 'docker compose up -d --build'
                    sh 'docker image prune -f'
                }
            }
        }

        stage('Database Update') {
            steps {
                sh 'docker cp /home/malde/projects/BackEnd/src/database_update.sql db:/tmp/db.sql'
                
                sh 'docker exec db psql -U admin -d postgres -f /tmp/db.sql'
                
                sh 'docker exec db rm /tmp/db.sql'
            }
        }
    }
}