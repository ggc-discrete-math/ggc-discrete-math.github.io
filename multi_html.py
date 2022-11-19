#!/usr/local/bin/py
import os, sys, copy, shutil
from bs4 import BeautifulSoup

def get_soup(html_file):
	return BeautifulSoup(open(html_file, 'rb').read(), 'html.parser')

# build master toc
#print('Hello')
os.system("asciidoctor-latex -b html " + 'book.adoc')

# get list of chapter files for asciidoctor
# chap_adocs = sorted([f for f in os.listdir() if f.startswith("chapter") and f.endswith(".adoc")])
chap_adocs = ['index.adoc', 'introduction_discrete_math.adoc', 'python_intro.adoc', 'logic.adoc', 'set_theory.adoc', 'functions.adoc', 'growth_functions.adoc', 'algorithms.adoc', 'counting.adoc', 'number_theory.adoc', 'induction_recursion.adoc', 'graph_theory.adoc', 'appendix_one.adoc']
chap_htmls = [f.replace('.adoc', '.html') for f in chap_adocs]

# go through master toc and update links to point to separate html files.
soup = get_soup('book.html')
toc_html = soup.find(id="toc")
anchors = toc_html.find_all('a')
chap_index = 0
for a in anchors:
	label = str(a.text)
	#print(label)
	if not (label.startswith(str(chap_index+1) + ".")):
		chap_index += 1
	a['href'] = chap_htmls[chap_index] + a['href']


# build copies of book with just 1 chapter in each.
for chap_index in range(0, len(chap_htmls)):
	print("Creating " + chap_htmls[chap_index])
	soup_copy = copy.copy(soup)
	loop_count = 0
	for div in soup_copy.find_all('div', {"class":"sect1"}):
		if loop_count != chap_index:
			div.decompose()
		loop_count += 1

	with open(chap_htmls[chap_index], "wb") as f_output:
			f_output.write(soup_copy.prettify("utf-8"))
			f_output.close()
