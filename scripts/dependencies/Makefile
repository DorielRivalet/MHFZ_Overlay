.PHONY: install_packages generate_images

install_packages:
	pip install -r ./scripts/dependencies/requirements.txt

generate_images:
	cd ./scripts && python commits_per_hour.py && python commits_per_weekday.py