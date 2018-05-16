#!/bin/bash
check=true
while $check; do
	read -p "Please enter a subject for the commit: " subject
	if [ ! -z "$subject" -a "$subject" != " " ]; then
		while true; do
			read -p "Are you sure you want the subject to be \"$subject\"? (y/n) " yn
			case $yn in
				[Yy]* ) check=false; break;;
				[Nn]* ) break;;
				* ) echo "Please answer \"y\" or \"n.\"";;
			esac
		done
	else
		echo "You cannot leave the subject blank."
	fi
done
echo $subject
git init
git remote add origin https://github.com/deliciousoctorok/dirty_fights.git
git add .
git commit -m "$subject"
git push -u origin andrew