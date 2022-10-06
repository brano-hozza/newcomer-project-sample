// eslint-disable-next-line no-undef
module.exports = {
	root: true,
	env: {
		browser: true,
		es6: true
	},
	extends: [
		'eslint:recommended',
		'plugin:@typescript-eslint/eslint-recommended',
		'plugin:@typescript-eslint/recommended',
		// 'requiring-type-checking' rules are not working with Vue right now
		//"plugin:@typescript-eslint/recommended-requiring-type-checking"
		'plugin:vue/recommended',
		'prettier'
	],
	globals: {
		Atomics: 'readonly',
		SharedArrayBuffer: 'readonly'
	},
	parser: 'vue-eslint-parser',
	parserOptions: {
		ecmaVersion: 2018,
		parser: '@typescript-eslint/parser',
		// 'requiring-type-checking' rules are not working with Vue right now
		//"project": "./tsconfig.json",
		sourceType: 'module'
	},
	// No need to specify the other plugins, extending the recommended rule sets does it for us
	plugins: ['simple-import-sort'],
	rules: {
		// ESLint
		'arrow-spacing': 'error',
		'arrow-parens': ['error', 'as-needed'],
		'arrow-body-style': 'error',
		'comma-dangle': 'error',
		curly: 'error',
		'default-case': 'error',
		'eol-last': 'error',
		eqeqeq: [
			'error',
			'always',
			{
				null: 'ignore'
			}
		],
		'keyword-spacing': [
			'error',
			{
				before: true,
				after: true
			}
		],
		'linebreak-style': ['error', 'windows'],
		'no-console': 'error',
		'no-debugger': 'error',
		'no-eval': 'error',
		'no-new-wrappers': 'error',
		'no-return-await': 'error',
		'no-throw-literal': 'error',
		'space-before-blocks': ['error', 'always'],
		yoda: [
			'error',
			'never',
			{
				exceptRange: true
			}
		],

		// Simple Import Sort - ESLint Plugin
		'simple-import-sort/imports': 'error',

		// TypeScript
		'@typescript-eslint/array-type': 'error',
		'@typescript-eslint/ban-types': [
			'error',
			{
				types: {
					Object: {
						message: 'Use object instead',
						fixWith: 'object'
					}
				}
			}
		],
		'@typescript-eslint/consistent-type-assertions': [
			'error',
			{
				assertionStyle: 'as',
				objectLiteralTypeAssertions: 'never'
			}
		],
		'@typescript-eslint/explicit-function-return-type': 'error',
		'@typescript-eslint/naming-convention': [
			'error',
			{
				selector: 'interface',
				format: ['PascalCase'],
				custom: {
					regex: '^I[A-Z]',
					match: true
				}
			}
		],
		'@typescript-eslint/member-ordering': [
			'error',
			{
				default: [
					'public-static-field',
					'public-instance-field',
					'protected-instance-field',
					'private-static-field',
					'private-instance-field',
					'public-constructor',
					'private-constructor',
					'public-static-method',
					'public-instance-method',
					'protected-instance-method',
					'private-static-method',
					'private-instance-method'
				]
			}
		],
		'@typescript-eslint/no-explicit-any': 'error',
		'@typescript-eslint/no-non-null-assertion': 'error',
		'@typescript-eslint/no-unused-vars': 'error',
		'no-useless-constructor': 'off',
		'@typescript-eslint/no-useless-constructor': 'error',
		indent: 'off',
		'@typescript-eslint/indent': ['error', 'tab'],
		semi: 'off',
		'@typescript-eslint/semi': ['error'],
		quotes: 'off',
		'@typescript-eslint/quotes': ['error', 'single'],

		// Vue
		'vue/html-closing-bracket-newline': [
			'error',
			{
				multiline: 'never'
			}
		]
	},
	overrides: [
		{
			files: ['*.json'],
			rules: {
				indent: ['error', 'tab'],
				'linebreak-style': ['error', 'windows'],
				quotes: ['error', 'double'],
				'@typescript-eslint/quotes': 0,
				'@typescript-eslint/semi': 0,
				semi: 0,
				'max-len': 0
			}
		}
	]
};
