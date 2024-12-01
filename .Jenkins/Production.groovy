def projectName = 'Devshift.Authentication.Api'
def gitUri = 'http://linuxdev02.nio.ngg.local/dotnet/Devshift.Authentication.Api.git'
def dockerTag = 'linuxdev02.nio.ngg.local:5000/Devshift.Authentication.Api'
def dockerComposeFile = 'Production/docker-compose.yml'

folder(projectName)
folder("$projectName/Production") {
    description 'The production environment is the environment in which the application runs when it is live and being used by end users.'
}

pipelineJob("$projectName/Production/Deploy_pmcs01n1") {
    logRotator(-1, 10)
    definition {
        parameters {
            stringParam('Tag', 'latest', 'Docker image tag for deploy.')
        }
        cps {
            sandbox()
            script("""
                @Library('jenkins-shared-libraries')_

                def _gitBranch = "refs/tags/\$Tag"
                def _dockerTag = '$dockerTag'.replaceAll(/:\\w+\$/, '') + ":\$Tag"

                netProduction {
                    remoteHost = 'pmcs01n1.nio.ngg.local'
                    gitUri = '$gitUri'
                    gitBranch = _gitBranch
                    projectName = '$projectName'
                    dockerTag = _dockerTag
                    dockerComposeFile = '$dockerComposeFile'
                }
            """)
        }
    }
}

pipelineJob("$projectName/Production/Deploy_pmcs02n2") {
    logRotator(-1, 10)
    definition {
        parameters {
            stringParam('Tag', 'latest', 'Docker image tag for deploy.')
        }
        cps {
            sandbox()
            script("""
                @Library('jenkins-shared-libraries')_

                def _gitBranch = "refs/tags/\$Tag"
                def _dockerTag = '$dockerTag'.replaceAll(/:\\w+\$/, '') + ":\$Tag"

                netProduction {
                    remoteHost = 'pmcs02n2.nio.ngg.local'
                    gitUri = '$gitUri'
                    gitBranch = _gitBranch
                    projectName = '$projectName'
                    dockerTag = _dockerTag
                    dockerComposeFile = '$dockerComposeFile'
                }
            """)
        }
    }
}

pipelineJob("$projectName/Production/Deploy_aws") {
    logRotator(-1, 10)
    definition {
        parameters {
            stringParam('Tag', 'latest', 'Docker image tag for deploy.')
        }
        cps {
            sandbox()
            script("""
                @Library('jenkins-shared-libraries')_

                def _gitBranch = "refs/tags/\$Tag"
                def _dockerTag = '$dockerTag'.replaceAll(/:\\w+\$/, '') + ":\$Tag"

                netProductionAws {
                    remoteHost = 'mcsn1.nggaws-local'
                    gitUri = '$gitUri'
                    gitBranch = _gitBranch
                    projectName = '$projectName'
                    dockerTag = _dockerTag
                    dockerComposeFile = '$dockerComposeFile'
                }
            """)
        }
    }
}

pipelineJob("$projectName/Production/Newman") {
    logRotator(-1, 10)
    definition {
        parameters {
            stringParam('Tag', 'latest', 'Git tag for build Docker image and push to Docker Registry.')
        }
        cps {
            sandbox()
            script("""
                @Library('jenkins-shared-libraries')_

                def _gitBranch = "refs/tags/\$Tag"

                newman {
                    gitUri = '$gitUri'
                    gitBranch = _gitBranch
                    environment = 'Production'
                }
            """)
        }
    }
}