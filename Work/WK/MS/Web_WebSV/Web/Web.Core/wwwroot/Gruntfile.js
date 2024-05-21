module.exports = function (grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        uglify: {
            common: {
                src: 'CTS/dist/js/jquery/jquery.extensions.js',
                dest: 'CTS/dist/js/jquery/jquery.extensions.min.js'
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');

    grunt.registerTask('default', ['uglify']);
}