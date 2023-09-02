# © 2023 The mhfz-overlay developers.
# Use of this source code is governed by a MIT license that can be
# found in the LICENSE file.

.PHONY: install_packages generate_images

install_packages:
	pip install -r requirements.txt

generate_images:
	cd .. && python commits_per_hour.py && python commits_per_weekday.py