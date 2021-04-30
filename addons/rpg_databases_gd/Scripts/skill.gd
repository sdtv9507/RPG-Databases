extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var icon_path: String = ""
var skill_selected: int = 0
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Skill")
	var system_dictionary: Dictionary = get_parent().get_parent().call("read_data", "System")
	$SkillButton.clear()
	for i in range(json_dictionary.size()):
		var skill_data: Dictionary = json_dictionary["skill"+String(i)]
		if i > $skillButton.get_item_count() - 1:
			$skillButton.add_item(skill_data["name"])
		else:
			$skillButton.set_item_text(i, skill_data["name"])
	var system_data: Dictionary = system_dictionary["elements"]
	for i in range(system_data.size()):
		if i >$DamageLabel/ElementLabel/ElementButton.get_item_count() - 1:
			$DamageLabel/ElementLabel/ElementButton.add_item(system_data[String(i)])
		else:
			$DamageLabel/ElementLabel/ElementButton.set_item_text(i, system_data[String(i)])
	system_data = system_dictionary["skills"]
	for i in range(system_data.size()):
		if i >$SkillTypeLabel/SkillTypeButton.get_item_count() - 1:
			$SkillTypeLabel/SkillTypeButton.add_item(system_data[String(i)])
		else:
			$SkillTypeLabel/SkillTypeButton.set_item_text(i, system_data[String(i)])
	refresh_data(skill_selected)

func refresh_data(id: int) -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Skill")
	var skill_data: Dictionary = json_dictionary["skill"+String(id)]
	$NameLabel/NameText.text = skill_data["name"]
	var icon: String = skill_data["icon"]
	if icon != "":
		icon_path = icon
		$IconLabel/IconSprite.texture = load(skill_data["icon"])
	$DescLabel/DescText.text = skill_data["description"]
	$SkillTypeLabel/SkillTypeButton.selected = skill_data["skill_type"]
	$MPCostLabel/MPCostBox.value = skill_data["mp_cost"]
	$TPCostLabel/TPCostBox.selected = skill_data["tp_cost"]
	$TargetLabel/TargetButton.selected = skill_data["target"]
	$UsableLabel/UsableButton.selected = skill_data["usable"]
	$HitLabel/HitBox.value = skill_data["success"]
	$TypeLabel/TypeButton.selected = skill_data["hit_type"]
	$DamageLabel/DTypeLabel/DTypeButton.selected = skill_data["damage_type"]
	$DamageLabel/ElementLabel/ElementButton.selected = skill_data["element"]
	$DamageLabel/DFormulaLabel/FormulaText.text = skill_data["formula"]
	if skill_data.has("effects"):
		clear_effect_list()
		var effect_list: Array = skill_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], int(effect["data_id"]), String(effect["value1"]), String(effect["value2"]))

func _on_Search_pressed() -> void:
	$IconLabel/IconSearch.popup_centered()

func _on_IconSearch_file_selected(path: String) -> void:
	icon_path = path
	$IconLabel/IconSprite.texture = load(path)

func _on_AddSkill_pressed() -> void:
	$SkillButton.add_item("NewSkill")
	var id: int = $SkillButton.get_item_count() - 1
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Skill")
	var skill_data: Dictionary
	skill_data["name"] = "NewSkill"
	skill_data["icon"] = ""
	skill_data["description"] = "New created skill"
	skill_data["skill_type"] = 0
	skill_data["mp_cost"] = 0
	skill_data["tp_cost"] = 0
	skill_data["target"] = 1
	skill_data["usable"] = 1
	skill_data["success"] = 95
	skill_data["hit_type"] = 1
	skill_data["damage_type"] = 1
	skill_data["element"] = 0
	skill_data["formula"] = "atk * 4 - def * 2"
	json_dictionary["skill" + String(id)] = skill_data
	get_parent().get_parent().call("store_data", "Skill", json_dictionary)

func _on_RemoveSkill_pressed():
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Skill")
	if json_dictionary.keys().size() > 1:
		var skill_id: int = skill_selected
		while skill_id < (json_dictionary.keys().size() - 1):
			json_dictionary["skill"+String(skill_id)] = json_dictionary["skill"+String(skill_id+1)]
			skill_id += 1
		json_dictionary.erase("skill"+String(skill_id))
		get_parent().get_parent().call("store_data", "Skill", json_dictionary)
		$SkillButton.remove_item(skill_selected)
		if skill_selected == 0:
			$SkillButton.select(skill_selected + 1)
			skill_selected += 1
		else:
			$SkillButton.select(skill_selected + 1)
			skill_selected -= 1
		$SkillButton.select(skill_selected)
		refresh_data(skill_selected)

func _on_SkillSaveButton_pressed() -> void:
	save_skill_data()
	refresh_data(skill_selected)

func save_skill_data() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Skill")
	var skill_data: Dictionary = json_dictionary["skill"+String(skill_selected)]
	var effect_list: Array
	skill_data["name"] = $NameLabel/NameText.text
	$SkillButton.set_item_text(skill_selected, $NameLabel/NameText.text)
	skill_data["icon"] = icon_path
	skill_data["description"] = $DescLabel/DescText.text
	skill_data["skill_type"] = $SkillTypeLabel/SkillTypeButton.selected
	skill_data["mp_cost"] = $MPCostLabel/MPCostBox.value
	skill_data["tp_cost"] = $TPCostLabel/TPCostBox.selected
	skill_data["target"] = $TargetLabel/TargetButton.selected
	skill_data["usable"] = $UsableLabel/UsableButton.selected
	skill_data["success"] = $HitLabel/HitBox.value
	skill_data["hit_type"] = $TypeLabel/TypeButton.selected
	skill_data["damage_type"] = $DamageLabel/DTypeLabel/DTypeButton.selected
	skill_data["element"] = $DamageLabel/ElementLabel/ElementButton.selected
	skill_data["formula"] = $DamageLabel/DFormulaLabel/FormulaText.text
	var effect_size: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_count()
	for i in effect_size:
		var effect_data: Dictionary
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_skill_text(i)
		effect_data["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_skill_text(i)
		effect_data["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.get_skill_text(i)
		effect_data["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.get_skill_text(i)
		effect_list.append(effect_data)
	skill_data["effects"] = effect_list
	get_parent().get_parent().call("store_data", "Skill", json_dictionary)

func _on_SkillButton_skill_selected(id: int) -> void:
	skill_selected = id
	refresh_data(id)

func _on_AddSkillEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Skill")

func _on_RemoveSkillEffect_pressed() -> void:
	var id: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)

func _on_CharacterAddEffectButton_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Character")

func _on_CharacterRemoveEffectButton_pressed() -> void:
	var id: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(String(data_id))
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.add_item(value1)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.add_item(value2)

func clear_effect_list() -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
